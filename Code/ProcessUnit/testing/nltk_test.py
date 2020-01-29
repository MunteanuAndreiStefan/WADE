from nltk.corpus import stopwords
from nltk.tokenize import word_tokenize
from nltk.tokenize import sent_tokenize
from nltk.stem import WordNetLemmatizer
import numpy as np
import os
import gensim
from googlesearch import search
from newspaper import Article

def sentence_tokenize(text):
    return sent_tokenize(text)

def tokenize(text):
    tokens = word_tokenize(text)
    words = [word for word in tokens if word.isalpha()]
    return words

def remove_stop_words(words):
    stop_words = set(stopwords.words('english')) 
    return [w for w in words if not w in stop_words]

def get_articles(text):

    keywords = remove_stop_words(tokenize(text))

    query = ''
    for k in keywords:
        query += k + ' '

    print(f'Query: {query}')

    data = []

    for url in search(query, tld='com', stop=10):

        print(url)

        article = Article(url)

        try:
            article.download()
            article.parse()
        except:
            continue
        
        # article.nlp()

        data.append(article)

    return data

def analyze(text):

    words = word_tokenize(text)
    sentences = sent_tokenize(text)

    words = [[w.lower() for w in word_tokenize(text)] for text in sentences]

    dictionary = gensim.corpora.Dictionary(words)

    # print(dictionary.token2id)

    corpus = [dictionary.doc2bow(words) for words in words]

    tfidf = gensim.models.TfidfModel(corpus)
    for doc in tfidf[corpus]:
        print([[dictionary [id], np.around(freq, decimals=2)] for id, freq in doc])

    if not os.path.exists('workdir'):
        os.mkdir('workdir')

    sims = gensim.similarities.Similarity('workdir/',tfidf[corpus], num_features=len(dictionary))

    results = []

    data = get_articles(text)

    for article in data:

        line = article.text

        query_doc = [w.lower() for w in word_tokenize(line)]
        query_doc_bow = dictionary.doc2bow(query_doc)

        query_doc_tf_idf = tfidf[query_doc_bow]
        
        print(sims[query_doc_tf_idf], article.title, np.average(sims[query_doc_tf_idf])) 

        results.append(np.max(sims[query_doc_tf_idf]))

    return int(max(results) * 100)

example = 'The 2019 Soul Train Music Awards are in the books, ending in a big night for Chris Brown and his song with Drake, "No Guidance," which took home three trophies. Here\'s the full list of winners.'

print(analyze(example))