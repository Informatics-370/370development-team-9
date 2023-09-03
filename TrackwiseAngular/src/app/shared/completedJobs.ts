export interface CompletedJob {
    job_Status_ID:string;
    deliveries:{
        job_ID:string,
        truckID:string,
        final_Mileage:number,
        initial_Mileage:number,
        totalFuel:number
    }[];

}