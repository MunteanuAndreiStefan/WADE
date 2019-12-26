import re
import json
import requests
from analyzers import TweetAnalyzerRandom, UserAnalyzerRandom
from config import Config as cfg
from logger import LogDecorator, log
from twitter_utils import get_tweet_data


class Manager():

    def __init__(self):

        self.tweet_analyzer = TweetAnalyzerRandom()
        self.user_analyzer = UserAnalyzerRandom()

    @LogDecorator()
    def generate_tweet_response(self, tweet_url, score, user_score, tweet_data):
        return {
            "url": tweet_url,
            "score": score,
            "user": tweet_data['user']['name'],
            "text": tweet_data['text'],
            "user_score": user_score
        }

    @LogDecorator()
    def analyze_tweet(self, tweet_url):

        # Extract username
        username = re.findall(r'twitter.com/(\w+)/status/', tweet_url)[0]

        # Lookup tweet info in cache
        '''
        content = requests.get(cfg.tweet_cache_url).content

        tweet_list = json.loads(content)

        for tweet in tweet_list:
            if tweet_url == tweet['source']:
                log(f"Tweet found in cache.")
                return self.generate_tweet_response(tweet_url, tweet['credibilityScore'], username)
        '''
        # Lookup username in cache
        content = requests.get(cfg.twitter_user_cache_url, headers={"X-Api-Key": cfg.x_api_key}).content
        users_list = json.loads(content)

        user_score = -1

        for user_info in users_list:
            # log(user_info)
            if username == user_info['username']:
                log(f"User found in cache.")
                user_score = user_info['credibilityScore']

        # Calculate user score
        if user_score == -1:
            user_score = self.user_analyzer.analyze(username)

            # Update cache
            user_cache_entry = {
                "id": 0,
                "username": username,
                "credibilityScore": user_score
            }

            json_user_entry = json.dumps(user_cache_entry)

            log(f"User not found in cache. Submitting new entry: {json_user_entry}")

            headers = {
                "Content-Type": "application/json",
                "Accept": "text/plain",
                "X-Api-Key": cfg.x_api_key
            }

            requests.post(cfg.twitter_user_cache_url, headers=headers, json=user_cache_entry)

        log(f"User score: {user_score}")

        # Calculate tweet score
        tweet_data = get_tweet_data(tweet_url)

        score = self.tweet_analyzer.analyze(tweet_data)
        log(f"Tweet score: {score}")

        # Adjust score based on user score
        score = int((0.75 * score) + (0.25 * user_score))
        log(f"Tweet score (user score adjusted): {score}")

        # Update cache
        cache_entry = {
            "id": 0,
            "source": tweet_url,
            "credibilityScore": score,
            "userId": 1
        }

        json_content = json.dumps(cache_entry)

        log(f"Tweet not found in cache. Submitting new entry: {json_content}")

        headers = {
            "Content-Type": "application/json",
            "Accept": "text/plain",
            "X-Api-Key": cfg.x_api_key
        }

        requests.post(cfg.tweet_cache_url, headers=headers, json=cache_entry)

        return self.generate_tweet_response(tweet_url, score, user_score, tweet_data)

    @LogDecorator()
    def analyze_user(self, user_id):
        return self.user_analyzer.analyze(user_id)
