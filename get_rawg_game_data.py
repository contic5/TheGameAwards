import pandas as pd
import os
from dotenv import load_dotenv
import time
import requests
import json

load_dotenv()

RAWG_API_KEY=os.getenv("RAWG_API_KEY")
start_row=0

def save_data(game_awards_and_rawg_list,rawg_all_list):

    print(f"Total Games: {len(game_awards_and_rawg_list)}")
    game_awards_and_rawg_df=pd.DataFrame.from_dict(game_awards_and_rawg_list)
    #game_awards_and_rawg_df.to_excel("Data/Game_Awards_Data_and_RAWG.xlsx")

    rawg_all_df=pd.DataFrame.from_dict(rawg_all_list)
    #rawg_all_df.to_excel("Data/RAWG_All.xlsx")

    with pd.ExcelWriter("Game_Awards_Data_and_RAWG.xlsx", engine="openpyxl") as writer:
        game_awards_and_rawg_df.to_excel(writer, sheet_name="Game_Awards", index=False)
        rawg_all_df.to_excel(writer, sheet_name="RAWG", index=False)

def main():
    game_awards_df=pd.read_excel("Data/Game_Awards_Data_Stable.xlsx")
    game_awards_df=game_awards_df.sort_values(by="Year",axis=0)
    #game_awards_and_rawg_df=pd.read_excel("Data/Game_Awards_Data_and_RAWG.xlsx")

    game_awards_and_rawg_list=[]
    rawg_all_list=[]

    for index,row in game_awards_df.iterrows():

        search_term=row["Nominee"]
        developer:str=row["Company"]
        developer=developer.lower()
        developer=developer.replace(" ","-")
        if row["IsGame"]:
            print(index,row["Nominee"],row["Year"],developer)
            response=requests.get("https://api.rawg.io/api/games",params={"key":RAWG_API_KEY,"search":search_term,"developers":[developer]})
            content=json.loads(response.content)
            if not content or len(content["results"])==0:
                print(index,row["Nominee"],row["Year"])
                response=requests.get("https://api.rawg.io/api/games",params={"key":RAWG_API_KEY,"search":search_term}) 

            content=json.loads(response.content)
            rawg_data={"ID":row["ID"]}
            rawg_data=rawg_data | content["results"][0]
            rawg_all_list.append(rawg_data)

            game_dict=row.to_dict()
            game_dict["metacritic"]=rawg_data["metacritic"]
            game_awards_and_rawg_list.append(game_dict)

            if index%10==0:
                save_data(game_awards_and_rawg_list,rawg_all_list)
            time.sleep(0.1)

    save_data(game_awards_and_rawg_list,rawg_all_list)

if __name__=="__main__":
    main()