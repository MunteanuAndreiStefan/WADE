import threading
import logging
import string
import time
import redis
import json
import re
import nltk
nltk.download('punkt')
nltk.download('stopwords')
nltk.download('sentiwordnet')
nltk.download('wordnet')
from nltk import word_tokenize
from nltk.corpus import stopwords
from nltk.corpus import sentiwordnet as swn

next(swn.all_senti_synsets())


logging.basicConfig(filename='process.log', filemode='a', level='INFO',
                    format='%(asctime)s - %(levelname)s - %(message)s')

redis_client = redis.StrictRedis(host='localhost', port=6379, db=0)

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
    print(preprocess_tweet(text))


def worker():
    while redis_client.llen('post') != 0:
        tweet = redis_client.lpop('post')
        process_tweet(tweet)


if __name__ == '__main__':
    for i in range(10):
        x = threading.Thread(target=worker)
        x.start()
