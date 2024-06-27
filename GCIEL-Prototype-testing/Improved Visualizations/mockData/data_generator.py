import pandas as pd
import random

# Define the pieces
pieces = [
    "Beam Pillar", "Blade", "Brace - Inside", "Brace - Outside", "Cross Beam", "Deck Board",
    "Front Stanchion Pole Base", "Front Stanchion Base", "Front Stanchion Cross Piece",
    "Front Stanchion Fish", "Front Stanchion Post", "Front Stancion Base Hole", "Gunwale",
    "Hull Mounting", "Keel", "Keelson Mesh", "Knee", "Upper Knee", "Mast", "Mast Lock",
    "Mast Stanchion Cross Piece", "Mast Stanchion", "Mastfish", "Mastfish Base", "Neck", "Oar",
    "Rear Stanchion Base", "Rear Stanchion Cross Piece", "Rear Stanchion Fish",
    "Rear Stanchion Pole Base", "Rear Stanchion Post", "Rear Stancion Base Hole", "Rib", "Strake", "Tiller"
]

# Define the number of players
players = [f'player{i}' for i in range(11)]

# Create an empty DataFrame
df = pd.DataFrame(columns=[
    'playerID', 'piece', 'completion_time', 'distance', 'watched_video', 'percentage_video', 'piece_look_time', 'hint'
])

# Populate the DataFrame
for player in players:
    for piece in pieces:
        completion_time = random.randint(1, 10)
        data = {
            'playerID': player,
            'piece': piece,
            'completion_time': random.uniform(10, 30),
            'distance': random.uniform(1, 4),
            'watched_video': random.choice([True, False]),
            'percentage_video': random.randint(0, 100),
            #make sure piece_look_time is less than the completion_time
            'piece_look_time': random.uniform(1, completion_time),
            'hint': random.choice([True, False])
        }
        df = df.append(data, ignore_index=True)

# Show a sample of the generated DataFrame
df.to_csv("viking_ship_data.csv")
