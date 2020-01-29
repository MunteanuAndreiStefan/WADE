import json
import pymongo
import redis

mongo_client = pymongo.MongoClient()
db = mongo_client.TwitterFakeNewsDetector
posts_collection = db.posts
users_collection = db.users

redis_client = redis.StrictRedis(host='localhost', port=6379, db=0)


def export_posts_collection():
    for post in posts_collection.find():
        del post['_id']  # not JSON serializable
        json_post = json.dumps(post)
        print(json_post, type(json_post))
        redis_client.lpush("post", json_post)


def dump_mongo_collection():
    posts = []
    for post in posts_collection.find():
        del post['_id']
        posts.append(post)
    with open("posts_collection.json", 'w') as fd:
        json.dump(posts, fd, indent=4)


if __name__ == '__main__':
    # export_posts_collection()
    dump_mongo_collection()
