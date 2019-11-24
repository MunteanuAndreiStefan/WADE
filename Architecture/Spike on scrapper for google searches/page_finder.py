import requests
from bs4 import BeautifulSoup
from tld.utils import update_tld_names
from urllib.parse import unquote

update_tld_names()

urlList = set()
urlList = { "https://www.google.com/search?q=profs+info+wade&ei=g-DBXbSzJ43orgTVxZToCg&start=0"  }

listOfWords = set(["note", "test", "edit", "response", "post", "send", "sign in", "reply", "review"])

doNotContain = set(["pdf", "xml", "doc", "docx", "xls", "txt", "gz"])

def printListToFile(filePath, list):
	f = open(filePath, 'a')
	print(*list, sep='\n', file=f)
	f.close()



for url in urlList:
    
    counter=0
    
    while(counter<110):
        
    
        page = requests.get(url)
        data = page.text
        soup = BeautifulSoup(data, features="html.parser")
        allTopics = soup.find_all('div', attrs={'class': 'g'})
        domains = set()
        
        linksToView = set()

        for topic in allTopics:
            link = topic.find('a', href=True)
            link = link['href']
            link=unquote(unquote(link))
            
            if("http" in str(link) or "https" in str(link)):
                link=link[7:]
                link=link.split("&")[0]
                print("\n"+link+"\n")
                ok=True
                
                for extension in doNotContain:
                    if(link.split(".")[-1].replace(" ", "") == extension or link in linksToView):
                        ok=False
                        break
                if(ok):
                    try:
                        openPage = requests.get(link)
                        openData = openPage.text
                        openSoup = BeautifulSoup(openData, features="html.parser")
                        text = (openSoup.text).lower()

                        for toTest in listOfWords:
                            if(toTest in text):
                                linksToView.add(link)
                                break
                    except:
                        print("An exception occurred on one page") 
                    
        counter=counter+10               
        url=url.split("start=")[0]+"start="+str(counter)
        print("------------------------------------   Page number:  "+ str(int(counter/10)) +"   ---------------------------------------------------------------")
        printListToFile("linksToView2.txt", linksToView)