export interface Truck {
    truckID: string;
    truck_License: string;
    model:String;
    mileage?:number;
    truck_Status_ID:string;
    truckStatus:{
        truck_Status_ID:string;
        status:string;
        description:string;
    };

}