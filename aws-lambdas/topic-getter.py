import json
from types import SimpleNamespace
import boto3
import botocore

dynamodb = boto3.resource('dynamodb')
table = dynamodb.Table('TABLE_NAME')


def lambda_handler(event, context):
    body = json.loads(event['body'], object_hook=lambda d: SimpleNamespace(**d))

    topic_id = body.Id
    try:
        if topic_id == '/':
            return send_response(get_all_topics_titles(), 200)

        return send_response(get_single_topic(topic_id), 200)

    except botocore.exceptions.ClientError as error:
        return send_response({}, 502)

def send_response(topics, status):
    return {
        'statusCode': status,
        'body': json.dumps(topics)
    }


def get_all_topics_titles():
    items = table.scan()['Items']
    titles = []

    for item in items:
        titles.append(item)

    return titles


def get_single_topic(id):
    topic = table.get_item(Key={'Id': id})

    return topic