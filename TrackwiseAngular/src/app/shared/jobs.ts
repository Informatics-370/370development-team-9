export interface Jobs
{
    job_ID:string,   
    startDate:Date,
    dueDate:Date,  
    pickup_Location:string,
    dropoff_Location:string,
    total_Weight:number,
    creator_ID:string

    jobStatus:{
        job_Status_ID : string,
        name : string,
        description: string 
    }

    jobType:{
        job_Type_ID : string,
        name : string,
        description: string 
    }

    Admin:{
        admin_ID: string;
        name: string;
        lastname:String;
        email:String;
        password: String;
    }

    Client:{
        client_ID: string;
        name: string;
        phoneNumber:String;
        email:string;
        password:string;
    }
}