# Developer Notes - Kelton Watts
#### April 9th, 2024

## Work Completed

- Completed 2 drafts of Location Animation Graph
  - Both graphs take time to upload.
  - Graph 1 uses ggplot.
    - Allows visualization of trail and focus on 1 player.
    - Could support toggling more players.
    - Cannot support play and pause button.
  - Graph 2 uses plotly.
    - Allows more control over animation rate and pausing on certain frames.
    - More control over animation style.
    - Uncertain of toggling players during animation.
    - Can toggle players on paused frames.
    - Potential for trails to be added.

## Next Steps

### Look Direction Animation

A visualization which shows the direction the player looks could pair well with the location animation
visualization to better understand player engagement and use patterns. The potential patterns could 
highlight if players are reading or just putting the boat together. 

### Data Needs

More data could lead to better visualizations and insights:
- Animation requires hms format for the time variable that is iterated upon. Changing Timestamp to 
fit hms format and adding a date attribute would create a more explicit data set to promote future animation.
  - Need to consider how to animate across multiple days.
  - Changing the Timestamp time from a clock time to a number of seconds since entrance to the game 
would allow for better comparison between players.
  - More frequently recorded data would lead to smoother and more accurate animations.
- X and Y coordinates for the direction the player is looking are necessary for the Look Direction Animation.
- X and Y coordinates for the boat, menu, next piece, etc. would allow annotations on the visualization 
to show where these objects of interest are located. Adding these annotations would allow for better context and insight to the importance of hot and cold zones on the heat map and location animation.

### UI Update

As more visualizations are added the UI is getting cluttered. Adding tabs to group visualizations by 
would lead to a more user friendly design. \
Each visualization also leaves no information about what to look for. If the creators of the visualization
are the audience the lack of context would be alright, but our audience is not limited to the visualization
authors. The visuals need to make sense to the development team, the artist team, the design team, 
subject matter experts, and those who are evaluating the effectiveness of our product both as a grant
recipient and experiment for the uses of VR in the classroom. If others cannot evaluate our product's
usefulness easily they will miss out on the actual product. To this end context for each visualization 
would greatly improve the impact of each visual and usability of the evaluation app.