import json
import boto3
import botocore

dynamodb = boto3.resource('dynamodb', region_name='us-east-2')
table = dynamodb.Table('TABLE_NAME')


def lambda_handler(event, context):

    try:
        remove_all()
        return send_response(200)

    except botocore.exceptions.ClientError as error:
        print(error)
        return send_response(502)

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
