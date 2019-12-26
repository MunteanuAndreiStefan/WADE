from nltk.corpus import stopwords
from nltk.tokenize import word_tokenize
from nltk.tokenize import sent_tokenize
from nltk.stem import WordNetLemmatizer
import numpy as np
import os
import gensim
from googlesearch import search
from newspaper import Article
from logger import LogDecorator, log


class SemanticAnalyzer:

    def __init__(self):
        pass

    @LogDecorator()
    def sentence_tokenize(self, text):
        return sent_tokenize(text)

    @LogDecorator()
    def tokenize(self, text):
        tokens = word_tokenize(text)
        words = [word for word in tokens if word.isalpha()]
        return words

    @LogDecorator()
    def remove_stop_words(self, words):
        stop_words = set(stopwords.words('english'))
        return [w for w in words if not w in stop_words]

    @LogDecorator()
    def get_articles(self, text):

        keywords = self.remove_stop_words(self.tokenize(text))

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

    @LogDecorator()
    def analyze(self, text):

        words = word_tokenize(text)
        sentences = sent_tokenize(text)

        words = [[w.lower() for w in word_tokenize(text)] for text in sentences]

        dictionary = gensim.corpora.Dictionary(words)

        # print(dictionary.token2id)

        corpus = [dictionary.doc2bow(words) for words in words]

        tfidf = gensim.models.TfidfModel(corpus)

        # for doc in tfidf[corpus]:
        #     print([[dictionary [id], np.around(freq, decimals=2)] for id, freq in doc])

        if not os.path.exists('workdir'):
            os.mkdir('workdir')

        sims = gensim.similarities.Similarity('workdir/', tfidf[corpus], num_features=len(dictionary))

        results = []

        data = self.get_articles(text)

        for article in data:
            line = article.text

            query_doc = [w.lower() for w in word_tokenize(line)]
            query_doc_bow = dictionary.doc2bow(query_doc)

            query_doc_tf_idf = tfidf[query_doc_bow]

            # print(sims[query_doc_tf_idf], article.title, np.average(sims[query_doc_tf_idf])) 

            article_score = int(np.max(sims[query_doc_tf_idf]) * 100)

            log(f'Score {article_score} for {article.url} {article.keywords}')

            results.append(article_score)

        return max(results)


if __name__ == '__main__':
    example = 'The 2019 Soul Train Music Awards are in the books, ending in a big night for Chris Brown and his song with Drake, "No Guidance," which took home three trophies. Here\'s the full list of winners.'
    anz = SemanticAnalyzer()
    print(anz.analyze(example))
