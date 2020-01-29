import random
from datetime import datetime, timezone, timedelta
from twitter_utils import get_tweet_data, get_user_data
from semantic_analyzer import SemanticAnalyzer
from logger import LogDecorator
from math import log


### Tweet analyzers

class TweetAnalyzer:

    def __init__(self):
        pass

    @LogDecorator()
    def get_tweet_content(self):
        # TODO get tweet content
        return ''

    def analyze(self, tweet_url):
        pass


class TweetAnalyzerRandom(TweetAnalyzer):

    @LogDecorator()
    def analyze(self, tweet_data):
        return random.randrange(0, 100)


class TweetAnalyzerHeuristic(TweetAnalyzer):

    @LogDecorator()
    def analyze(self, tweet_data):
        return 0


class TweetAnalyzerML(TweetAnalyzer):

    @LogDecorator()
    def analyze(self, tweet_data):
        return 0


class TweetAnalyzerSemantic(TweetAnalyzer):

    @LogDecorator()
    def analyze(self, tweet_data):
        sman = SemanticAnalyzer()
        score = sman.analyze(tweet_data['text'])

        in_reply_status_id = tweet_data['in_reply_status_id']
        if in_reply_status_id:
            second_semantic_analyzer = SemanticAnalyzer()
            in_reply_tweet_data = get_tweet_data(in_reply_status_id)
            additional_score = 1 * second_semantic_analyzer.analyze(in_reply_tweet_data['text'])
            score += additional_score

        return score


### User analyzers

class UserAnalyzer:

    def __init__(self):
        pass

    def analyze(self, user_id):
        user_data = get_user_data(user_id)
        score = 0

        if user_data['location'] != "":
            score += 5

        now = datetime.now(tz=timezone.utc)
        created_at = datetime.strptime(user_data['created_at'], "%a %b %d %H:%M:%S %z %Y")
        delta = now - created_at
        score += log(delta.days)

        score += log(user_data['followers_count'])
        score += log(user_data['friends_count'])
        score += log(user_data['statuses_count'])
        score += log(user_data['listed_count'])

        if user_data['verified']:
            score += 500

        return score


class UserAnalyzerRandom(UserAnalyzer):

    @LogDecorator()
    def analyze(self, username):
        return random.randrange(0, 100)


if __name__ == '__main__':
    # a = UserAnalyzer()
    # a.analyze("DavidHenigUK")
    a = TweetAnalyzerSemantic()
    a.analyze("https://twitter.com/RaniaKhalek/status/1213863134009679872")
