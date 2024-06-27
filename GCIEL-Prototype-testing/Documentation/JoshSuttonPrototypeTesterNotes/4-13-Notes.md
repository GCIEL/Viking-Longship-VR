# Developer Notes - Josh Sutton
#### April 13th, 2024

## Work Completed

- Shift of data table to main
  - Previous work done by Ethan moved to main with the addition of descriptions
- Addition of google form tab
    - tabPanel made for ARCS model based Evaluation form
    - Form removed from data vis tab
- Disable of data vis tab with no file input
    - disable class no initialization
    - observe for when file input is not null then enable data vis tab
    - helpful shinyjs disable discussion https://stackoverflow.com/questions/64324152/shiny-disable-tabpanel-using-shinyjs