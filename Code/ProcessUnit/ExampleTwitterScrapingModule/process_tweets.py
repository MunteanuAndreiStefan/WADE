import threading
import logging
import string
import time
import redis
import json
import re
import math
import nltk
nltk.download('punkt')
nltk.download('stopwords')
nltk.download('sentiwordnet')
nltk.download('wordnet')
from nltk import word_tokenize
from nltk.corpus import stopwords
from nltk.corpus import sentiwordnet as swn
from rake_nltk import Metric, Rake
import tfidf



next(swn.all_senti_synsets())


logging.basicConfig(filename='process.log', filemode='a', level='INFO',
                    format='%(asctime)s - %(levelname)s - %(message)s')

redis_client = redis.StrictRedis(host='localhost', port=6379, db=0)

def compute_term_frequency(word_dict, bow):
    tf_dict = {}
    bow_count = len(bow)
    for word, count in word_dict.items():
        tf_dict[word] = count/float(bow_count)
    return tf_dict

def compute_inverse_data_frequency(doc_list):
    idf_dict = {}
    n = len(doc_list)
    idf_dict = dict.fromkeys(doc_list[0].keys(), 0)
    for doc in doc_list:
        for word, val in doc.items():
            if val > 0:
                idf_dict[word] += 1

    for word, vald in idf_dict.items():
        idf_dict[word] = math.log10(n/float(val))

    return idf_dict

def preprocess_tweet(text):
    # Check characters to see if they are in punctuation
    nopunc = [char for char in text if char not in string.punctuation]
    # Join the characters again to form the string.
    nopunc = ''.join(nopunc)
    # convert text to lower-case
    nopunc = nopunc.lower()
    # remove URLs
    nopunc = re.sub('((www\.[^\s]+)|(https?://[^\s]+)|(http?://[^\s]+))', '', nopunc)
    nopunc = re.sub(r'http\S+', '', nopunc)
    # remove usernames
    nopunc = re.sub('@[^\s]+', '', nopunc)
    # remove the # in #hashtag
    nopunc = re.sub(r'#([^\s]+)', r'\1', nopunc)
    # remove repeated characters
    nopunc = word_tokenize(nopunc)
    # remove stopwords from final word list
    return [word for word in nopunc if word not in stopwords.words('english')]

def process_tweet(tweet):
    tweet = json.loads(tweet)
    text = tweet["text"]
    print(tfidf.get_word_frequencies(text))
    print(preprocess_tweet(text))


def worker():
    while redis_client.llen('post') != 0:
        tweet = redis_client.lpop('post')
        process_tweet(tweet)


if __name__ == '__main__':
    for i in range(10):
        x = threading.Thread(target=worker)
        x.start()
