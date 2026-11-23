import pandas as pd
import json

def main():
    game_awards_df=pd.read_excel("Data/Game_Awards_Data_and_RAWG_Stable.xlsx",sheet_name="RAWG")
    genres_list=[]

    for index,row in game_awards_df.iterrows():
        print(row["name"])

        genres=[]

        genres_raw=row["genres"]
        genres_raw=genres_raw.replace("'",'"')
        print(genres_raw)

        genres_json=json.loads(genres_raw)
        print(genres_json)
        for item in genres_json:
            genres.append(item["name"])

        genres_written=",".join(genres)
        genre_dict={"ID":row["ID"],"Nominee":row["name"],"Genres":genres_written}
        genres_list.append(genre_dict)

    genres_df=pd.DataFrame.from_dict(genres_list)
    genres_df.to_excel("Data/Game_Awards_Genres.xlsx",index=False)

if __name__=="__main__":
    main()