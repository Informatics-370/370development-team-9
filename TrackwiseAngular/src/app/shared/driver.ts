export interface Driver {
    driver_ID: string;
    name: string;
    lastname:String;
    phoneNumber:String;
    driver_Status_ID:string;
    driverStatus:{
        driver_Status_ID:string;
        status:string;
        description:string;
    };

}