from lxml import html
import requests
import nltk
from SPARQLWrapper import SPARQLWrapper, JSON, XML
from bs4 import BeautifulSoup
import tweepy
import os
from flask import Flask, request

nltk.download('punkt')
nltk.download('averaged_perceptron_tagger')

app = Flask(__name__)

@app.route("/")
def home():
    return "Hello!"


@app.route('/procent_1')
def procent_1():
    url = request.form['url']

    # Get Twitter text from url
    page = requests.get(url)
    tree = html.fromstring(page.content)
    text = \
    tree.xpath('//div[contains(@class, "permalink-tweet-container")]//p[contains(@class, "tweet-text")]//text()')[0]

    # Find de subject
    sentences = nltk.sent_tokenize(text)
    nouns = []

    for sentence in sentences:
        for word, pos in nltk.pos_tag(nltk.word_tokenize(str(sentence))):
            if (pos == 'NN' or pos == 'NNP' or pos == 'NNS' or pos == 'NNPS'):
                nouns.append(word)

    # SPARQL Validation
    procent_1_true = 0
    procent_1_false = 0

    for noun in nouns:
        sparql = SPARQLWrapper("http://dbpedia.org/sparql")
        sparql.setQuery("""
            ASK WHERE {
                <http://dbpedia.org/resource/""" + noun + """> rdfs:label "  """ + noun + """  "@en
            }
        """)
        sparql.setReturnFormat(XML)
        results = sparql.query().convert()

        soup = BeautifulSoup(results.toxml(), features="lxml")
        print(soup.find('boolean').string)
        if (soup.find('boolean').string == "false"):
            procent_1_false += 1
        else:
            procent_1_true += 1

    return str(10 * float(procent_1_false)/float(10))

@app.route('/procent_2')
def procent_2():
    url = request.form['url']

    # Get Twitter text from url
    page = requests.get(url)
    tree = html.fromstring(page.content)
    text = \
        tree.xpath('//div[contains(@class, "permalink-tweet-container")]//p[contains(@class, "tweet-text")]//text()')[0]

    # Find de subject
    sentences = nltk.sent_tokenize(text)
    nouns = []

    for sentence in sentences:
        for word, pos in nltk.pos_tag(nltk.word_tokenize(str(sentence))):
            if pos == 'NN' or pos == 'NNP' or pos == 'NNS' or pos == 'NNPS':
                nouns.append(word)

    # Search word on Twitter
    auth = tweepy.OAuthHandler("WxrZB58GN8L9q8KSKwqNKuaap", "xREexZIFzlpnicQvnA2tlrqwdzDkUGyMpzaoEkaReiTvpqUFsJ")
    auth.set_access_token("847102303945547777-E6kFwZK8z99hbu6qAy36KQCnAsiJEbX",
                          "YiEjDzz86yhj0OofrFYCTfdDeXkDufEvV0FA1KDgdEfuT")

    api = tweepy.API(auth)

    full_text_array = []
    full_text_subject = []

    count = 0
    for page in tweepy.Cursor(api.search, q='Trump', count=2, tweet_mode='extended').pages():
        # process status here
        # data = json.loads(page)
        # print(data['extended_tweet']['full_text'])
        print(page[0]._json['full_text'])
        full_text_array.append(page[0]._json['full_text'])
        count += 1
        if count > 3:
            break

    nouns_twitter = []
    for text in full_text_array:
        sentences = nltk.sent_tokenize(text)

        for sentence in sentences:
            for word, pos in nltk.pos_tag(nltk.word_tokenize(str(sentence))):
                if pos == 'NN' or pos == 'NNP' or pos == 'NNS' or pos == 'NNPS':
                    nouns_twitter.append(word)

    procent_1_true = 0
    for t1 in nouns:
        for t2 in nouns_twitter:
            if t1 == t2:
                procent_1_true += 1

    return str(10 * float(procent_1_true)/float(10) + 10)

@app.route('/procent_3')
def procent_3():
    url = request.form['url']

    # Get Twitter text from url
    page = requests.get(url)
    tree = html.fromstring(page.content)
    text = \
        tree.xpath('//div[contains(@class, "permalink-tweet-container")]//p[contains(@class, "tweet-text")]//text()')[0]

    # Find de subject
    sentences = nltk.sent_tokenize(text)
    nouns = []

    for sentence in sentences:
        for word, pos in nltk.pos_tag(nltk.word_tokenize(str(sentence))):
            if pos == 'NN' or pos == 'NNP' or pos == 'NNS' or pos == 'NNPS':
                nouns.append(word)

    # Make SPARQL Interogation
    nouns_twitter = []
    sparql = SPARQLWrapper("http://dbpedia.org/sparql")
    for noun in nouns:
        sparql.setQuery("""
            PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
            SELECT ?info
            WHERE { <http://dbpedia.org/resource/"""+noun+"""> rdfs:comment ?info }
        """)
        sparql.setReturnFormat(JSON)
        results = sparql.query().convert()

        for result in results["results"]["bindings"]:
            res = result["info"]["value"]
            sentences = nltk.sent_tokenize(res)

            for sentence in sentences:
                for word, pos in nltk.pos_tag(nltk.word_tokenize(str(sentence))):
                    if pos == 'NN' or pos == 'NNP' or pos == 'NNS' or pos == 'NNPS':
                        nouns_twitter.append(word)

    procent_1_true = 0
    for t1 in nouns:
        for t2 in nouns_twitter:
            if t1 == t2:
                procent_1_true += 1

    return str(10 * float(procent_1_true)/float(10) )

# https://schema.org/AnalysisNewsArticle?fbclid=IwAR1KLAosINVZTp3rfHNcvdSSdedfeMIBQtcpi2Pw1_DAFHudyzAypuBgYdc
@app.route('/procent_4')
def procent_4():
    url = request.form['url']

    # Get Twitter text from url
    page = requests.get(url)
    tree = html.fromstring(page.content)
    text = \
        tree.xpath('//div[contains(@class, "permalink-tweet-container")]//p[contains(@class, "tweet-text")]//text()')[0]

    # Find de subject
    sentences = nltk.sent_tokenize(text)
    nouns = []

    for sentence in sentences:
        for word, pos in nltk.pos_tag(nltk.word_tokenize(str(sentence))):
            if pos == 'NN' or pos == 'NNP' or pos == 'NNS' or pos == 'NNPS':
                nouns.append(word)

    sparql = SPARQLWrapper("http://dbpedia.org/sparql")
    sparql.setQuery("""
    PREFIX dbo: <http://dbpedia.org/ontology/>
    SELECT DISTINCT 
    ?AnalysisNewsArticle ?AnalysisNewsArticle_name ?AnalysisNewsArticle_creator 
    WHERE{
      ?AnalysisNewsArticle a dbo:Person.
      ?AnalysisNewsArticle dbo:birthPlace ?AnalysisNewsArticle_name.
      ?AnalysisNewsArticle dbo:almaMater ?AnalysisNewsArticle_creator.
    }
    LIMIT 40
    """)
    sparql.setReturnFormat(JSON)
    results = sparql.query().convert()

    sparql_urls = []
    for result in results["results"]["bindings"]:
        sparql_urls.append(os.path.basename(result["AnalysisNewsArticle_name"]["value"]))

    procent_1_true = 0
    for noun in nouns:
        for url_name in sparql_urls:
            if url_name == noun:
                procent_1_true += 1

    return str(10 * float(procent_1_true)/float(10) )

if __name__ == "__main__":
    app.run(debug=True)