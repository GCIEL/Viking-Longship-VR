# Loading the required libraries

library(shinyjs)
library(shiny)

library(ggplot2)
library(corrplot)
library(reshape2)
library(plotly)

library(dplyr)
library(stringr)
library(readxl)


# Setting the Shiny option for maximum request size
options(shiny.maxRequestSize = 100 * 1024^2)


# Defining the user interface

ui <- fluidPage(
  
  ## Our custom HTML/CSS for the UI theme
  tags$head(
    tags$style(HTML('
        }
        .animated-title {
          animation: fadeIn 1s; /* Use fadeIn animation for 1 second */
        }
        @keyframes fadeIn {
          from {
            opacity: 0;
          }
          to {
            opacity: 1;
          }
        }
      '))
  ),
  
  ## The heading to be displayed on the app
  titlePanel(
    h1("GCIEL Assessment Strategy", style = "color: #aa66cc; font-size: 60px;", class = "animated-title")
  ),
  
  ## Using the Shiny Sidebar Layout
  sidebarLayout(
    sidebarPanel(
      
      ## Instructional text for users
      HTML('<p style="color: #aa66cc;">Please ensure to review the data 
           description before uploading a CSV file. Make sure that the dataset 
           you upload matches the data description in order for the app to work 
           correctly.</p>'),
      
      fileInput("file",
                label = "Upload your dataset (CSV)",
                accept = c("text/csv"),
                multiple = FALSE,
                width = "80%"),
      
      # Adding a link to download the PDF file
      downloadLink("dataDescriptionLink", "Download Data Description", class = "download-link"),
      br(),
      downloadLink("vikingshipDataLink", "Download Data (CSV)", class = "download-link"),
    ),
    
    ## Creating the framework for output tables, graphs, and evaluation form
    mainPanel(
      tabsetPanel(type = "tabs",
                  tabPanel("Data", 
                           h1("Data", style = "color: aa66cc;"),
                           dataTableOutput("outFile")),
                  tabPanel("Total Completion Time per Piece", 
                           h1("Completion Time Analysis", style = "color: aa66cc;"), 
                           plotlyOutput("completionTimePlot")),
                  tabPanel("Video Engagement Analysis", 
                           h1("Average Percentage of Video Watched per Piece", style = "color: aa66cc;"), 
                           plotlyOutput("videoEngagementPlot")),
                  tabPanel("3D Distance Analysis by Piece", 
                           h1("Distance vs Completion Time", style = "color: aa66cc;"), 
                           plotlyOutput("distancePlot2"), 
                           p(strong("Guide -->")), 
                           tags$ol(
                             tags$li(tags$b("Selecting a particular piece - "), "Double click on the piece in the legend."),
                             tags$li(tags$b("Adding another piece to it - "), "Single click on the other piece in the legend."),
                             tags$li(tags$b("Removing a piece - "), "Single click on the piece in the legend."),
                             tags$li(tags$b("Adding all the pieces back - "), "Double click anywhere on the legend.")), 
                           p("Information on operating the graph is in the bar above the legend. 
                             Simply hover over the icons to learn more.")),
                  tabPanel("3D Distance Analysis by Player", 
                           h1("Distance vs Completion Time", style = "color: aa66cc;"), 
                           plotlyOutput("distancePlot1"), 
                           p(strong("Guide -->")), 
                           tags$ol(
                             tags$li(tags$b("Selecting a particular player - "), "Double click on the player in the legend."),
                             tags$li(tags$b("Adding another player to it - "), "Single click on the other player in the legend."),
                             tags$li(tags$b("Removing a player - "), "Single click on the player in the legend."),
                             tags$li(tags$b("Adding all the players back - "), "Double click anywhere on the legend.")), 
                           p("Information on operating the graph is in the bar above the legend. 
                             Simply hover over the icons to learn more.")),
                  tabPanel("Player Positions Heatmap",
                           h1("Player Positions Heatmap", style = "color: #aa66cc;"),
                           plotlyOutput("heatmapPlot")),
                  tabPanel("ARCS Model Based Evaluation",
                           htmlOutput("googleFormTab"))
      )
    )
  )
)

# Defining the server logic
server <- function(input, output, session) {
  
  ## Reactive expression for the uploaded file
  inFile <- reactive({
    tmp <- input$file
    if (is.null(tmp)) {
      return(NULL)
    } else {
      df <- read.csv(tmp$datapath, header = TRUE)
      return(df)
    }
  })
  
  ## 1. Rendering the uploaded file in a data table
  output$outFile <- renderDataTable({
    data.frame(inFile())
  })
  
  
  ## 2. Plot for Completion Time Analysis
  output$completionTimePlot <- renderPlotly({
    
    req(inFile())
    
    p <- ggplot(inFile(), aes(x = piece, y = completion_time)) +
      geom_bar(stat = "summary", fun = "sum", fill = "#aa66cc") +
      theme_minimal() +
      theme(axis.text.x = element_text(angle = 45, hjust = 1)) +
      labs(x = "Piece", y = "Total Completion Time")
    
    ggplotly(p)
  })
  
  
  ## 3. Plot for Video Engagement Analysis
  output$videoEngagementPlot <- renderPlotly({
    
    req(inFile())
    
    p <- ggplot(inFile(), aes(x = piece, y = percentage_video)) +
      geom_bar(stat = "summary", fun = "mean", fill = "#aa66cc") +
      theme_minimal() +
      theme(axis.text.x = element_text(angle = 45, hjust = 1)) +
      labs(x = "Piece", y = "Average Percentage Watched")
    
    ggplotly(p)
  })
  
  
  ## 4. Plot for Distance Analysis by Piece
  output$distancePlot2 <- renderPlotly({
    
    req(inFile())
    
    plot_ly(data = inFile(), type = "scatter3d", mode = "markers", 
            x = ~distance, y = ~completion_time, z = ~piece, color = ~piece,
            marker = list(size = 3), text = ~playerID)
  })
  
  
  ## 5. Plot for Distance Analysis by Player
  output$distancePlot1 <- renderPlotly({
    
    req(inFile())
    
    plot_ly(data = inFile(), type = "scatter3d", mode = "markers",
            x = ~distance, y = ~completion_time, z = ~playerID, color = ~playerID,
            marker = list(size = 3), text = ~piece)
    
    # p <- ggplot(inFile(), aes(x = distance, y = completion_time, color = piece)) +
    #   geom_point() +
    #   theme_minimal() +
    #   labs(x = "Distance", y = "Completion Time")
    # 
    # ggplotly(p)
    
  })
  
  ## 6. Plot for the heatmap
  output$heatmapPlot <- renderPlotly({
    
    heatmap <- ggplot(inFile(), aes(x = X, y = Y, fill = ..density..)) +
      geom_bin2d(bins = 30) +
      scale_fill_viridis_c() +
      labs(title = "2D Heatmap of Player Positions", x = "X Coordinate", y = "Y Coordinate")
  })
  
  
  ## 7. Rendering the Google Form in the "Google Form" tab
  output$googleFormTab <- renderUI({
    HTML('<iframe src="https://docs.google.com/forms/d/e/1FAIpQLSeUbp57sYcssoH45croShmzwlVomcMjLJ-xJHMAlTpB4cBC5Q/viewform?embedded=true" width="100%" height="800" frameborder="0" marginheight="0" marginwidth="0">読み込んでいます…</iframe>')
  })
  
  
  ## 8. Defining download link for data description PDF
  output$dataDescriptionLink <- downloadHandler(
    filename = function() {
      "dataDescription.pdf"
    },
    content = function(file) {
      file.copy("dataDescription.pdf", file)
    })
  
  
  ## 9. Defining download link for the dataset in CSV format
  output$vikingshipDataLink <- downloadHandler(
    filename = function() {
      "vikingshipData.csv"
    },
    content = function(file) {
      file.copy("vikingshipData.csv", file)
    })
}


# Launch the Shiny app
shinyApp(ui, server)
