import json
import boto3

dynamodb = boto3.resource('dynamodb', region_name='us-east-2')
table = dynamodb.Table('TABLE_NAME')


def lambda_handler(event, context):

    remove_all()
    return send_response(200)

def send_response(status):
    return {
        'statusCode': status
    }

def remove_all():
    scan = table.scan()

    with table.batch_writer() as batch:
        for each in scan['Items']:
            batch.delete_item(Key={
                'Id': each['Id']
            })
