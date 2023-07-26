export interface Driver {
    driver_ID: string;
    name: string;
    lastname:String;
    phoneNumber:String;
    email:string;
    password:string;
    driver_Status_ID:string;
    driverStatus:{
        driver_Status_ID:string;
        status:string;
        description:string;
    };

}