#Fake news detection on twitter
Project for WADE subject on Distribuited System master

Functional Requirements:

    Information will be collected from Twitter (posts, user information (number of posts, folowers, account creation date, etc.))
    User profiles will be built (characterized by details about the user, user activity, reactions of other users to what he does on the network, etc.)
    A metric will be proposed to calculate the credibility of a post
    A metric will be proposed for credibility of a user
    A metainformation should be taken into consideration
    Methods will be proposed to detect "fake" users and "fake" news
    We could have a list of fake users centralized in order to get a better response, after the front-end is putting the user in a category
    Resources we will be centralized in order to get faster responses for already processed news
    Resources will be used outside the Twitter network to validate the information (Google, blogs, newspapers, etc.)
    Fronted will be able to show extra information about metrics
    Fronted will be able to edit the stric level of the marking
    Fronted will show analized resources
    All users will be unique
    User posibility to mark a post as fake manually

Actors:

    Any user that will have the plugin installed

Use Cases:

    User will be able to install the plugin from a store
    User will be able to disable/enable the application.
    User will be able to set the maximum/minimum level of marking a news as fake
    User will be able to see results from a news
    User will be able to see what resources we found
    User will be able to open those resources

Non-Functional Requirements:

    Cache centralized system for all the users
    Nothing on security side for the final product due it's created only for academic purposes
    The application will run for chrome and firefox
    Level of trust per user, calculated with a Bayes/ML/Statistical methods over semantic or non semantic data
    Take advantage of posts metadata
    "Layers of trust" in order to say as fast as possible if a post is fake or not
    Parser with BS4 in order to get information from trusted websites
    Implement a crawler maybe with search engines in order to find credible sources

Final components:

    Frontend: the plugin
    Backend: a highway for our cache system.
    Process unit: on the frontend via js, compatible languages and on microservices if needed more horse power
    Crawling: a component for crawling on search engines/engine
    Parser: in order to get information from lists of documents found
    Training unit: collect cache information and create model if needed for process unit
    Twitter collector: delivers the data to the process unit
  
Team Component:
  - Munteanu Andrei-Ștefan
  - Victor Vlad
  - Pantelemon Victor-Stefan
  
  Link to our blog: http://students.info.uaic.ro/~stefan.munteanu/WADE/
