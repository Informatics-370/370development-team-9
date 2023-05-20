export interface Driver {
    driver_ID: number;
    name: string;
    lastname:String;
    phoneNumber:String;
    driver_Status_ID:number;
    driverStatus:{
        driver_Status_ID:number;
        status:string;
        description:string;
    };

}