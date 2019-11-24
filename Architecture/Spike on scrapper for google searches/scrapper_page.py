from bs4 import BeautifulSoup, SoupStrainer
import requests
from urllib.parse import urlparse
from tld.utils import update_tld_names
update_tld_names()

urlList = set()
urlList = { "https://profs.info.uaic.ro"  }
listOfWords = set(["uaic", "info", "profs", "wade",  "test", "note"])

def printListToFile(filePath, list):
	f = open(filePath, 'w')
	print(*list, sep='\n', file=f)
	f.close()

for url in urlList:
    
    page = requests.get(url)    
    data = page.text
    soup = BeautifulSoup(data, features="html.parser")
    allLinks = soup.find_all('a')
    domains = set()


    for link in allLinks:
        if("http" in str(link) or "https" in str(link)):
            parsed_uri = urlparse(link.get('href'))
            result = '{uri.scheme}://{uri.netloc}/'.format(uri=parsed_uri)
            domainWithSub=result.split("//")[-1].split("/")[0]
            domain=result.split("//")[0]+ "//" + '.'.join(domainWithSub.split(".")[-2:]) + "/"
            domains.add(domain)

    domainChecked = []

    for link in domains:
        print("Try domain: " + link)
        try:
            page = requests.get(link)
            data = page.text
            soup = BeautifulSoup(data, features="html.parser")
            text = (soup.text).lower()
            for toTest in listOfWords:
                if(toTest in text):
                    domainChecked.append(link)
                    break
        except:
            print("An exception occurred on one page") 

    printListToFile("Competitors.txt", domainChecked)