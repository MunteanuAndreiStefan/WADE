import random
from twitter_utils import get_tweet_data
from semantic_analyzer import SemanticAnalyzer
from logger import LogDecorator

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

        return score

### User analyzers

class UserAnalyzer:
    
    def __init__(self):
        pass

    def analyze(self, user_id):
        pass
    
   class UserAnalyzerSemantic(UserAnalyzer):

    @LogDecorator()
    def analyze(self, tweet_data):

        sman = SemanticAnalyzer()
        score = sman.analyze(user_data['text'])

        return score
