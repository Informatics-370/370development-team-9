export interface JobDetailDTO {
    job_ID: string;
    total_Weight: number;
    total_Trips: number;
    pickup_Location: string;
    dropoff_Location :string;
    startDate: Date;
    dueDate: Date;
    deliveryList: deliveryDetailDTO[]; // Make sure the property name matches your data
  }
  
  export interface deliveryDetailDTO {
    delivery_ID:string;
    delivery_Weight: number;
    trips: number;
  }