export interface Job {
    job_ID:string,   
    startDate:string,
    dueDate:string,  
    pickup_Location:string,
    dropoff_Location:string,
    total_Weight:number,
    creator_ID:string,
    job_Type_ID : string,
    job_Status_ID: string,
    userName?:string
}