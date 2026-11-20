import pandas as pd
from bs4 import BeautifulSoup
import requests
import time

def save_data(game_list):
    print(f"Total Games: {len(game_list)}")
    game_df=pd.DataFrame.from_dict(game_list)
    game_df.to_excel("Data/Game_Awards_Data.xlsx",index=False)

def main():
    game_list=[]
    id_num=1
    start_year=2026

    for year in range(2014,2027):
        if year<start_year:
            continue

        headers = {"User-Agent": "MyResearchScript/1.0 (myemail@gmail.com)"}
        response=requests.get(f"https://en.wikipedia.org/wiki/The_Game_Awards_{year}",headers=headers)
        soup=BeautifulSoup(response.text,features="lxml")

        #print(soup.prettify(encoding="utf-8"))
        wikitable=soup.find("table",class_="wikitable")
        tds=wikitable.find_all("td")
        ths=wikitable.find_all("th")

        categories=[]
        for th in ths:
            categories.append(th.text)

        for i in range(len(tds)):
            lis=tds[i].find_all("li")

            #Get winner data from first item
            if year<2026:
                b_elements=lis[0].find_all("b")

                winner=b_elements[0].text
                winner_company=b_elements[0].text
                if len(b_elements)>1:
                    winner_company=b_elements[1].text

                game_dict={"ID":id_num,"Nominee":winner,"Company":winner_company,"Category":categories[i],"Year":year,"Winner":True}
                game_list.append(game_dict)
                id_num+=1

                #Start from first item if winner is known
                start=1
            else:
                #Otherwise start from first item
                start=0
            for li in lis[start:]:
                text=li.text
                parts=text.split("–")
                if len(parts)==1:
                    parts.append(parts[0])
                parts=[part.strip() for part in parts]

                game_dict={"ID":id_num,"Nominee":parts[0],"Company":parts[1],"Category":categories[i],"Year":year,"Winner":False}
                game_list.append(game_dict)
                id_num+=1
        save_data(game_list)
        time.sleep(1)
    save_data(game_list)

if __name__=="__main__":
    main()