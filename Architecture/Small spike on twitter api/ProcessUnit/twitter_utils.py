from datetime import datetime

class TwitterAPI():
    """
        Used as adapter between ProcessUnit manager and Twitter API Handler Module.
    """

    def __init__():
        pass

    def get_tweet_by_id(id):
        pass

    def get_tweet_by_url(url):
        pass

    def get_user_by_username(username):
        pass


class UserHandle():
    """
        Used to model a Twitter user.
    """

    def __init__(username, date_joined):

        self.username = username
        self.date_joined = date_joined


class TweetHandle():
    """
        Used to model a tweet.
    """

    def __init__(id, text, date_posted, author_username):

        self.id = id
        self.text = text
        self.date_posted = date_posted
        self.author_username = author_username

