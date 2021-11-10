import json
from types import SimpleNamespace
import boto3

dynamodb = boto3.resource('dynamodb', region_name='us-east-2')
table = dynamodb.Table('TABLE_NAME')


def lambda_handler(event, context):
    body = json.loads(event['body'], object_hook=lambda d: SimpleNamespace(**d))

    put_topic(body.Id, body.Author, body.Title, body.Text)

    return send_response(200)

def send_response(status):
    return {
        'statusCode': status
    }

def put_topic(id, author, title, text):

    table.put_item(
        Item={
            'Id': id,
            'Author': author,
            'Title': title,
            'Text': text
        }
    )
