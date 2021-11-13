
**Author**: Gabriel Przybysz Gonçalves Júnior - Backend Programmer <br>

# **How I created my personal 4chan**

Forant Anonymous Topics is based/inspired by the famous 4Chan. In it, users post random topics anonymously, where the newest ones are above the old ones. However, on Forant Anonymous Topics, daily, topics are automatically deleted. An interface is simple and, image upload is not allowed. Creating it was 100% free for testing (Using the AWS Cloud's free tier services.)

# Flowchart and overview
The application was created using ASP.NET, which allows the creation of dynamic pages, together with AWS Cloud, allowing the creation of REST APIs (API Gateway), a NoSQL database (DynamoDB), and Lambdas (Python 3.8). The application's core is the data requests for the API Gateway using the Services.cs class. The REST APIs make a call to a specific lambda of put, get, or comment. With the information received, it is possible to create dynamic pages, listing comments, topics, authors... Using a Topic object, which shapes the elements brought from the database, which has the following structure:
|Parameter| Value|
|--|--|
| **Id** | A Guid that looks like this: be5d82ffff744f86ac817c39dc0d8582 |
| **Nickname** | Field filled in by the user at the time of topic creation
| **Title** | Topic title, filled in by the user at the time of topic creation
|**Text** |   The content of the topic, filled in by the user at the time of topic creation
| **Comments** | Anonymous comments on the topic in question

And daily, using the AWS Cloud's EventBridge service, the daily-remover lambda is called, completely cleaning the database (DynamoDB).

For better visualization of the communication, see the flowchart:

![](https://lh6.googleusercontent.com/bqqntdP_KcNC-ITKWRs7XlYqe2vFY4kFpDBZdceHON6VH-KaH8kPMYkIGqAIVBOIebjiJyxzjwz92s9nk-eMryhJDoUTeLeGLfz4Is8dffHXaO0n-90Uu5JcOCLwgdueCu13BDIx)
## ASP.NET Technology

“ASP.NET offers three frameworks for building Web applications: Web Forms, ASP.NET MVC, and ASP.NET Web Pages. All three frameworks are stable and mature, and you can build great web apps with any of them. Regardless of which framework you choose, you'll get all the benefits and features of ASP.NET everywhere.
Each framework targets a different development style. What you choose depends on a combination of your programming assets (development knowledge, skills, and experience), the type of application you are building, and the development approach you are familiar with.
Below is an overview of each of the structures and some ideas on how to choose between them. If you prefer an introduction to the video, see Making Sites With ASP.net and What Are Web Tools?” - Explanation taken from Microsoft documentation.


###  Binding Method

An example of binding is the one done for our Nickname field:

**HTML:**
```html
<input type="text" asp-for="Nickname" placeholder="Your Nick">
```

**Controller:** 
```csharp
[BindProperty] public string Nickname { get; set; } = "";
```

So when the post method is called the values ​​are mapped to the string Nickname. "This means action methods will have one or more parameters and those parameters will receive their values ​​from the model binding framework." http://www.binaryintellect.net/articles/85fb9a1d-6b0d-4d1f-932c-555bd27ba401.aspx


## Services.cs

It is used to create and send HTTP requests in a fully async way.
We can see an example when creating a new topic:

```csharp
public async Task PutTopic(Topic newTopic)
{
   WebRequest request = WebRequest.Create ("https://url.execute-api.us-east-2.amazonaws.com/default/topic-setter");
   request.Method = "POST";
   request.ContentType = "application/json";
   request.Headers.Add("x-api-key","key");
  
   using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync()))
   {
       string json = JsonConvert.SerializeObject(newTopic);
       await streamWriter.WriteAsync(json);
   }

   await request.GetResponseAsync();
}
```
It makes a request to the topic-setter, which creates a new topic in DynamoDB and returns a statuscode 200.

## Lambdas

To create the lambdas follow these steps:

 1. **Inside of console, look for the lambda service** <br>
![](https://lh3.googleusercontent.com/w-DWV9RObC3TIOUNywYfg-HXbri6FZcuK090EGl3hkNj2hpIvIV4fJ6IGhTDGzB-ORfbYvu_6gj_IXqXZF_KdlgqWtr9FfxHezJD4bDJb2S8sRXj9GrkYUxt2g3ZKM96Oqo66KJp)
 <br>
 
 
 2.  **Go to "Create a function"** <br>

![](https://lh5.googleusercontent.com/T4Zdim5GI2sCGoJ0jPzrKfyxzKCMMiOi-zbkzUwPJmgOGxl5wTOkYAdVR06qTtFoyCLMJedzvwYY2zSfaLTibc1EqxMVKsmN6WX4D9dGs4WvH4-7xDbwCXRSnHyWMP4aqm-ByPj4)
<br>

 3. **Set the following informations:**
   
		1.  Function Name
    
		2.  Use architecture x86_64
    
		3.  Choose the language to use to write your function. (Python3.8)
    
		4.  To create roles, let it create automatically and change it later

4. **After creating it, go to Configurations > Permissions > Click on your Role**
5.   **Add policies > add “AmazonDynamoDBFullAccess”**
 

## *Code*

### Topic-Getter
In this lambda we can have two different types of responses. The first one, sent a
“Id = /” in the request body, to receive all topics. The second sent a valid id thus receiving a specific topic. The code can be seen at: https://github.com/GabrielPrzybysz/forant-anonymous-topics/blob/main/aws-lambdas/topic-getter.py

### Topic-Setter
It receives the following object serialized by the Newtonsoft library in its body:
```csharp
public class Topic
{

	public Topic(string id, string author, string title, string text , List<string> comments)

	{

		Id = id;

		Author = author;

		Title = title;

		Text = text;

		Comments = comments;

	}

	public string Id { get; set; }

  

	public string Author { get; set; }

	public string Title { get; set; }

	public string Text { get; set; }

	public List<string> Comments { get; set; }

}
```

Thus, performing a put operation for DynamoDB, adding one more daily topic.

### Daily-Remover
  It scans the entire dynamo table daily and using boto3's batch_writer feature, it deletes item by item


# API Gateway

All my REST APIs have an api-key, thus making the process more secure, and also in them, there is a limit of requests per second, thus avoiding any attack of multiple requests.

 1.   **In the tab of your lambda, look for the "Add Trigger"** 
<br>

![](https://lh6.googleusercontent.com/zge7ic17fdKw4XfWAaU2MJ8vaUz72jzb9X2kFeUq8YJ4MniTfxFEgSaEFnaMSUgf7K6oLPnMeQU9qGCDMIOPkkhZ0xWiS1QHOWnZkbKp0TQ83iFkcRB-NnmMpu_I5IqxEMt81FwA)
<br>


 2.   **When choosing the trigger configuration, choose "API Gateway"**
 <br>
 
![](https://lh3.googleusercontent.com/DkrgiJs1TyEzgzyXBpo_DdEDc-Oh4VDBGJw6PDYcyw7XXWCCIibwyVChXC_MraqFIeHGa1YITdReWxQzmkNEfEpxApah8K-0_2K4dttNTPb9QVDxyjV9t6RLTV0bdzvqACOSyCnX)
 <br>
 
 
 3. **Configure following the following image** 
 <br>
 
![](https://lh3.googleusercontent.com/ZvK9MfmZQItVcr8i_sVFetxbQaR-iPiER0eNbDliq4N3093QV0FScdkNRavUyds5grotffKxrpCNoEArQPgWXvMKiHsX8XbFVTKSJfT-tHjaVo_jx10oGTbDT4y87m9iqGC8XIOW)
 <br>