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

library(gif)
library(gifski)
library(gganimate)
library(DT)


# Setting the Shiny option for maximum request size
options(shiny.maxRequestSize = 100 * 1024 ^ 2)


css <- "
.nav li a.disabled {
background-color: #FFF !important;
color: #eee !important;
cursor: not-allowed !important;
border-color: #aaa !important;
}
"


#Adding visualization descriptions (These are brief and need to be modernized).
#Update made 4.11.24
dynamicDescription <- function(vis){
  switch(vis,
         "Total Completion Time per Piece" = "This bar graph gives the total 
         time (seconds) of all players taken to successfully place each piece.
         This bar graph is a good indicator for tracking learning progress. The 
         longer it takes to complete a piece indicates a longer amount spent 
         looking at the website browser, while still looking at how long the 
         user might've taken to look at the piece directly. This also means that 
         pieces looked at longer on average give info on which piece is more 
         popular than others and thus can find possible reasons as to why.",
         
         "2D Distance Analysis by Piece" = "This plot gives a comparison to the 
         distance a certain piece was moved from its origin to the time taken to 
         place piece. This graph gives a visual understanding to how fast or 
         slow all player moved while holding a chosen piece.",
         
         "2D Distance Analysis by Player" = "This plot displays each player and how 
         each piece was interacted with comparing the distance moved from
         its origin to the time taken to place the piece. This graph gives a 
         visual understanding to how fast or slow one player moved each piece.",
         
         "Player Positions Heatmap" = "This heatmap gives the density of all player 
         positions within the X and Z axis of the VR experience.",
         
         "Player Location Animation 1" = "This graph displays the player 
         location over time along the X and Z axis of the VR experience.",
         
         "Player Location Animation 2" = "This graph displays all player 
         locations over time along the X and Z axis of the VR experience.",
         
         "Item View Amount" = "This graph displays how long each item in VR was 
         looked at by each player in seconds. This graph may show user 
         progression of time spent observing each item. The progression can 
         help indicate where players look more giving an indicator on locations 
         that might need to be adjusted to increase view times. During the 
         optimization process the graph could be helpful in indicating where interactive implementations might need to be added 
         or where models might need to be enhanced."
  )
}


# Defining the user interface
ui <- shinyUI(
  fluidPage(
    shinyjs::useShinyjs(),
    shinyjs::inlineCSS(css),
    ## Our custom HTML/CSS for the UI theme
    tags$head(tags$style(
      HTML(
        '
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
      '
      )
    )),
    navbarPage(
      title = "GCIEL Assessment Strategy",
      tabPanel(
        "Input Data",
        ## Instructional text for users
        uiOutput("instructionText"), 
        
        fileInput(
          "file",
          label = "Upload your dataset (CSV)",
          accept = c("text/csv"),
          multiple = FALSE,
          width = "80%"
        ),
        
        # Adding a link to download the PDF file
        downloadLink("dataDescriptionLink", "Download Data Description", class = "download-link"),
        br(),
        downloadLink("vikingshipDataLink", "Download Data (CSV)", class = "download-link"),
        
        # Data Table
        renderDT("outFile", options = list(
          lengthChange = TRUE, pageLength = 10
        )),
        dataTableOutput("outFile")
      ),
      
      tabPanel(
        "Data Vis",
        
        # Specify layout for side panel.
        # Side panel is used for modifying graphs using drop down bars and sliders.
        sidebarLayout(
          sidebarPanel(
            # Side panel for time graph
            conditionalPanel(
              # Check the  graph is selected
              condition = "input.tabs == 'use' && input.tabs_use == 'completion'",
              ns = NS(NULL),
              
              # Panel title
              h3("Total Completion Time per Piece"),
              # Context
              h5(dynamicDescription("Total Completion Time per Piece"))
            ),
            
            # Side panel for time graph
            conditionalPanel(
              # Check the  graph is selected
              condition = "input.tabs == 'use' && input.tabs_use == '2dpiece'",
              ns = NS(NULL),
              
              # Panel title
              h3("2D Distance Analysis by Piece"),
              # Context
              h5(dynamicDescription("2D Distance Analysis by Piece"))
            ),
            
            # Side panel for time graph
            conditionalPanel(
              # Check the  graph is selected
              condition = "input.tabs == 'use' && input.tabs_use == '2dplayer'",
              ns = NS(NULL),
              
              # Panel title
              h3("2D Distance Analysis by Player"),
              # Context
              h5(dynamicDescription("2D Distance Analysis by Player"))
            ),
            conditionalPanel(
              # Check the  graph is selected
              condition = "input.tabs == 'use' && input.tabs_use == 'viewAmt'",
              ns = NS(NULL),
              
              # Panel title
              h3("Item View Amount"),
              # Context
              h5(dynamicDescription("Item View Amount"))
            ),
            
            # Side panel for time graph
            conditionalPanel(
              # Check the  graph is selected
              condition = "input.tabs == 'heat' && input.tabs_heat == 'heat'",
              ns = NS(NULL),
              
              # Panel title
              h3("Player Positions Heatmap"),
              # Context
              h5(dynamicDescription("Player Positions Heatmap"))
            ),
            
            # Side panel for time graph
            conditionalPanel(
              # Check the  graph is selected
              condition = "input.tabs == 'heat' && input.tabs_heat == 'loc1'",
              ns = NS(NULL),
              
              # Panel title
              h3("Player Location Animation 1"),
              # Context
              h5(dynamicDescription("Player Location Animation 1")),
              uiOutput("locationAnimationControls"),
              actionButton("updateAnimationButton", "Update Graph"),
              h5("Instructions: Select players and then click \"Update Graph\"
                 to see the graph.")
            ),
            
            # Side panel for time graph
            conditionalPanel(
              # Check the  graph is selected
              condition = "input.tabs == 'heat' && input.tabs_heat == 'loc2'",
              ns = NS(NULL),
              
              # Panel title
              h3("Player Location Animation 2"),
              # Context
              h5(dynamicDescription("Player Location Animation 2"))
            )
            
          ),
          
          mainPanel(
            tabsetPanel(
              type = "tabs",
              # id is used for conditionals in side panels
              id = "tabs",
              tabPanel(
                # id and value used for conditionals in side panels
                id = "use",
                value = "use",
                
                # Panel title
                "User Engagement Visualizations",
                
                # Individual panels
                tabsetPanel(
                  # id and value used for conditionals in side panels
                  type = "tabs",
                  id = "tabs_use",
                  tabPanel(
                    "Total Completion Time per Piece",
                    plotlyOutput("completionTimePlot"),
                    value = "completion"
                  ),
                  tabPanel(
                    "2D Distance Analysis by Piece",
                    plotlyOutput("distancePlot2"),
                    value = "2dpiece"
                  ),
                  tabPanel(
                    "2D Distance Analysis by Player",
                    plotlyOutput("distancePlot1"),
                    value = "2dplayer"
                  ),
                  tabPanel(
                    fig.width=7,
                    "Player Piece Viewing",
                    imageOutput("viewAmountAnimation"),
                    value = "viewAmt"
                  )
                  
                )
              ),
              tabPanel(
                # id and value used for conditionals in side panels
                id = "heat",
                value = "heat",
                
                # Panel title
                "Heatmap Visualizations",
                
                # Individual panels
                tabsetPanel(
                  # id and value used for conditionals in side panels
                  type = "tabs",
                  id = "tabs_heat",
                  tabPanel(
                    "Player Positions Heatmap",
                    plotlyOutput("heatmapPlot"),
                    value = "heat",
                  ),
                  tabPanel(
                    "Player Location GIF Animation",
                    imageOutput("locationAnimation1"),
                    value = "loc1"
                  ),
                  tabPanel(
                    "Player Location VideoAnimation",
                    plotlyOutput("locationAnimation2"),
                    value = "loc2"
                  )
                )
              )
            ))
        ),
      ),
      tabPanel("ARCS Model Evaluation",
               htmlOutput("googleFormTab"))
      
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
      df <- read.csv(tmp$datapath, header = TRUE) %>% 
        # distance from piece generation spot at each location
        mutate(distance = sqrt((-0.75 - X)^2 +(1.5-Z)^2)) %>%
        # shift head position to adjust for camera positioning
        mutate(X = X+5.3) %>%
        mutate(Z = Z-7.4)
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
    
    # get Completion Time
    df <- inFile() %>% group_by(piece) %>%
      summarise(totalCount = n()) %>% 
      mutate(completion_time = totalCount * .5)
    
    p <- ggplot(df, aes(x = piece, y = completion_time)) +
      geom_bar(stat = "summary",
               fun = "sum",
               fill = "#aa66cc") +
      theme_minimal() +
      theme(axis.text.x = element_text(angle = 45, hjust = 1)) +
      labs(x = "Piece", y = "Total Completion Time (Seconds)")
    
    ggplotly(p)
  })
  
  
  #  ## 3. Plot for Video Engagement Analysis
  #  output$videoEngagementPlot <- renderPlotly({
  #    req(inFile())
  #    
  #    p <- ggplot(inFile(), aes(x = piece, y = percentage_video)) +
  #      geom_bar(stat = "summary",
  #               fun = "mean",
  #               fill = "#aa66cc") +
  #      theme_minimal() +
  #      theme(axis.text.x = element_text(angle = 45, hjust = 1)) +
  #      labs(x = "Piece", y = "Average Percentage Watched")
  #    
  #    ggplotly(p)
  #  })
  
  
  ## 4. Plot for Distance Analysis by Piece
  output$distancePlot2 <- renderPlotly({
    req(inFile())
    
    #get Distance
    dfLastPos <- inFile() %>%
      group_by(piece) %>%
      slice(1)
    
    # get Completion Time
    dfComplete <- inFile() %>% group_by(piece) %>%
      summarise(totalCount = n()) %>% 
      mutate(completion_time = totalCount * .5)
    
    # Combine the two
    dfCombine <- dfComplete %>%
      inner_join(dfLastPos, by = "piece")
    
    plot_ly(
      data = dfCombine,
      type = "scatter",
      mode = "markers",
      x = ~ distance,
      y = ~ completion_time,
      color = ~ piece,
      marker = list(size = 8),
      text = ~ playerID
    )
  })
  
  
  ## 5. Plot for Distance Analysis by Player
  output$distancePlot1 <- renderPlotly({
    req(inFile())
    
    #get Distance
    dfLastPos <- inFile() %>%
      group_by(piece) %>%
      slice(1)
    
    # get Completion Time
    dfComplete <- inFile() %>% group_by(piece) %>%
      summarise(totalCount = n()) %>% 
      mutate(completion_time = totalCount * .5)
    
    # Combine the two
    dfCombine <- dfComplete %>%
      inner_join(dfLastPos, by = "piece")
    
    plot_ly(
      data = dfCombine,
      type = "scatter",
      mode = "markers",
      x = ~ distance,
      y = ~ completion_time,
      color = ~ playerID,
      marker = list(size = 10),
      text = ~ piece
    )
    
    # p <- ggplot(inFile(), aes(x = distance, y = completion_time, color = piece)) +
    #   geom_point() +
    #   theme_minimal() +
    #   labs(x = "Distance", y = "Completion Time")
    #
    # ggplotly(p)
    
  })
  
  
  ## 6. Plot for the heatmap
  output$heatmapPlot <- renderPlotly({
    # Data frame for the locations of interactables
    locations <- data.frame(
      X = c(-0.6, -0.6, 0.6, 0.6, -1.2, -0.75),
      Z = c(-1, 1, -1, 1, -2.4, 1.5),
      Labels = c("Boat", "Boat", "Boat", "Boat", "Menu", "Next Piece")
    )
    
    # Create the heatmap
    heatmap <- ggplot(inFile(), aes(x = Z, y = X)) +
      geom_bin2d(aes(fill = after_stat(density)), bins = 30) +
      scale_fill_viridis_c() +
      #Label Locations
      labs(title = "2D Heatmap of Player Positions", 
           x = "Z Coordinate", 
           y = "X Coordinate",
           color='Labels',
           fill="")+
      geom_point(data = locations, 
                 aes(x = Z, y = X, color = Labels), 
                 size = 4) +
      scale_x_reverse()
    
    # Convert ggplot object to plotly to make it interactive
    ggplotly(heatmap)
  })
  
  
  
  ## 7. Rendering the Google Form in the "Google Form" tab
  output$googleFormTab <- renderUI({
    HTML(
      '<iframe src="https://docs.google.com/forms/d/e/1FAIpQLSeUbp57sYcssoH45croShmzwlVomcMjLJ-xJHMAlTpB4cBC5Q/viewform?embedded=true" width="100%" height="1000" frameborder="0" marginheight="0" marginwidth="0">読み込んでいます…</iframe>'
    )
  })
  
  
  ## 8. Defining download link for data description PDF
  output$dataDescriptionLink <- downloadHandler(
    filename = function() {
      "dataDescription.pdf"
    },
    content = function(file) {
      file.copy("dataDescription.pdf", file)
    }
  )
  
  
  ## 9. Defining download link for the dataset in CSV format
  output$vikingshipDataLink <- downloadHandler(
    filename = function() {
      "vikingshipData.csv"
    },
    content = function(file) {
      file.copy("vikingshipData.csv", file)
    }
  )
  
  
  ## Begin Spring 2024 Server Work
  ## 10. Menu Select for choosing which players to compare in location animation 1
  ## Export Checkbox Menu for location animation 1
  output$locationAnimationControls <- renderUI({
    req(inFile())
    checkboxGroupInput(inputId = "playerSelect",
                       label = "Select Players to Visualize:",
                       choices = unique(inFile()$playerID))
  })
  
  
  ## Update location animation graph choices
  ## Button must be clicked for visual to appear, wraps 11 and 12
  observeEvent(input$updateAnimationButton, {
    playerIDs <- input$playerSelect
    
    
    ## 11. Define location animation draft 1 plot
    animateLocation <- reactive({
      
      data <- inFile() %>% filter(playerID %in% playerIDs)
      # Ensure that your data frame has a sequence column for ordering
      data$Sequence <- seq_len(nrow(data))
      
      # Data frame for the locations of interactables
      locations <- data.frame(
        X = c(-0.6, -0.6, 0.6, 0.6, -1.2, -0.75),
        Z = c(-1, 1, -1, 1, -2.4, 1.5),
        Labels = c("Boat", "Boat", "Boat", "Boat", "Menu", "Next Piece")
      )
      
      ggplot(data, aes(x = Z,
                       y = X,
                       color = playerID)) +
        transition_time(Sequence) +  # Use Sequence as frames for ordering
        labs(
          subtitle = "Timestamp: {frame_time}",
          x = "Z Coordinate",
          y = "X Coordinate",
          title = "Player Location Animation through Play Time",
          color = "Entities and Interactables"
        ) +
        geom_point(size = 6) +
        # Interactable locations
        geom_point(data = locations, 
                   aes(x = Z, y = X, color = Labels), 
                   size = 6) +
        # Font Sizes
        theme(
          plot.title = element_text(size = 20, face = "bold"),
          axis.title = element_text(size = 14), 
          axis.text = element_text(size = 12),  
          legend.title = element_text(size = 18),
          legend.text = element_text(size = 12)) + 
        # Flip x axis to match played experience
        scale_x_reverse() + 
        shadow_wake(wake_length = 0.1, alpha = 0.5)
    })
    
    
    ## 12. Export location animation draft 1
    output$locationAnimation1 <- renderImage({
      # Generate the plot
      locationPlot <-
        animate(animateLocation(),
                nframes = 100,
                width = 800, 
                height = 500,
                renderer = gifski_renderer("location.gif"))
      
      # Return the image
      list(src = "location.gif", deleteFile = TRUE)
    }, deleteFile = FALSE) # Ensure deleteFile is set to FALSE to keep the image file
  })  # End wrap of reactive button
  
  
  ## 13. Define and Export location animation draft 2
  output$locationAnimation2 <- renderPlotly({
    req(inFile())
    
    plot_ly(
      data = inFile(),
      x = ~ X,
      y = ~ Z,
      size = 3,
      color = ~ playerID,
      frame = ~ Timestamp,
      # text = ~country,
      # hoverinfo = "text",
      type = 'scatter',
      mode = 'markers'
    ) %>%
      animation_opts(frame = 2000,
                     easing = "linear",
                     redraw = FALSE) %>%
      layout(title = "Player Location Animation Through Time")
  })
  
  
  #14. Enable Data Vis Tab
  shinyjs::disable(selector = '.navbar-nav a[data-value="Data Vis"')
  isFileValid <- reactive({
    if (is.null(inFile())) {
      return(TRUE)  # File is not valid
    } else {
      # Check if the required columns are present in the uploaded file
      if ("X" %in% colnames(inFile()) && 
          "Z" %in% colnames(inFile()) && 
          "playerID" %in% colnames(inFile()) &&
          "Timestamp" %in% colnames(inFile()) &&
          "piece" %in% colnames(inFile())) {
        shinyjs::enable(selector = '.navbar-nav a[data-value="Data Vis"')
        return(TRUE)  # File is valid
      } else {
        shinyjs::disable(selector = '.navbar-nav a[data-value="Data Vis"')
        return(FALSE)  # File is not valid
      }
    }
  })
  
  
  #15. Item View Amount Bar Chart Rendering
  output$viewAmountAnimation <- renderImage({
    # Get Time Each item viewed
    dfView <- inFile() %>% 
      group_by(Look_Item, playerID) %>%
      summarise(totalCount = n()) %>%
      mutate(piece_look_time = totalCount * 0.5,
             ID = strtoi(str_sub(playerID, start = 7))) %>%
      select(playerID, Look_Item, piece_look_time, ID)
    
    plot <- ggplot(dfView, aes(ID, group = playerID, 
                               fill = as.factor(playerID), 
                               color = as.factor(playerID))) +
      geom_tile(aes(y = piece_look_time/2,
                    height = piece_look_time,
                    width = 0.9), color = NA) +
      geom_text(aes(y = 0, label = paste(playerID, "  ")), 
                vjust = 0.2, hjust = 1, size = 8) +
      geom_text(aes(y = piece_look_time, label = piece_look_time, 
                    hjust = 0, size = 8)) +
      coord_flip(clip = "off", expand = FALSE) +
      scale_y_continuous(labels = scales::comma) +
      scale_x_reverse() +
      guides(color = FALSE, fill = FALSE) +
      theme(plot.background = element_blank(),
            axis.line = element_blank(),
            axis.text.x = element_blank(),
            axis.text.y = element_blank(),
            axis.ticks = element_blank(),
            axis.title.x = element_blank(),
            axis.title.y = element_blank(),
            legend.position = "none",
            panel.background = element_blank(),
            panel.border = element_blank(),
            panel.grid.major = element_blank(),
            panel.grid.minor = element_blank(),
            plot.title = element_text(size = 25, hjust = 0.5, face = "bold", vjust = -1),
            plot.subtitle = element_text(size = 18, hjust = 0.5, face = "italic"),
            plot.margin = margin(2, 2, 2, 4, "cm"))
    
    animatedPlot <- plot + transition_states(Look_Item, transition_length = 10, state_length = 2) +
      labs(title = 'Viking Ship Piece : {closest_state}',  
           subtitle  =  "Item View Amount (Seconds)")
    
    animate(animatedPlot, 500, fps = 10,  width = 800, height = 500, 
            renderer = gifski_renderer("viewAmt.gif"))
    
    list(src = "viewAmt.gif", deleteFile = TRUE)
  }, deleteFile = FALSE)
  
  
  # Render Instructions of file input if invalid
  output$instructionText <- renderUI({
    if (isFileValid()) {
      HTML(
        '<p style="color: #aa66cc;">Please ensure to review the data
        description before uploading a CSV file. Make sure that the dataset
        you upload matches the data description in order for the app to work
        correctly.</p>'
      )
    } else {
      HTML(
        '<p style="color: red;">Invalid Data Set Uploaded</p>'
      )
    }
  })
}

# Launch the Shiny app
shinyApp(ui, server)
