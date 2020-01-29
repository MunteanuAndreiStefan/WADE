import requests
from bs4 import BeautifulSoup
from tld.utils import update_tld_names
from urllib.parse import unquote

update_tld_names()

urlList = set()
urlList = { "https://www.google.com/search?q=profs+info+wade&start=0"  } #google search link to be iterated into.

def startApplication(startGoogleUrl, listOfWords, doNotContain):
    if doNotContain is None:
        doNotContain = set(["pdf", "xml", "doc", "docx", "xls", "txt", "gz"])
    startScan(startGoogleUrl, doNotContain, listOfWords )
    return result

def printListToFile(filePath, list):
	f = open(filePath, 'a')
	print(*list, sep='\n', file=f)
	f.close()

def startScan(urlList, shouldNotContain, wordsToSearch):
    for url in urlList:
        
        counter=0
        
        while(counter<110): #number of pages to look into // 10 pages hardcoded.
        
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
                    
                    for extension in shouldNotContain:
                        if(link.split(".")[-1].replace(" ", "") == extension or link in linksToView):
                            ok=False
                            break
                    if(ok):
                        try:
                            openPage = requests.get(link)
                            openData = openPage.text
                            openSoup = BeautifulSoup(openData, features="html.parser")
                            text = (openSoup.text).lower()

                            for toTest in wordsToSearch:
                                if(toTest in text):
                                    linksToView.add(link)
                                    break
                        except:
                            print("An exception occurred on one page") 
                        
            counter=counter+10               
            url=url.split("start=")[0]+"start="+str(counter)
            print("------------------------------------   Page number:  "+ str(int(counter/10)) +"   ---------------------------------------------------------------")
            printListToFile("linksFound.txt", linksToView)