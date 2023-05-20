export interface Truck {
    truckID: number;
    truck_License: string;
    model:String;
    truck_Status_ID:number;
    truckStatus:{
        truck_Status_ID:number;
        status:string;
        description:string;
    };

}