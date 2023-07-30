export interface Job {
    job_ID: string;
    startDate: Date;
    dueDate: Date;
    pickupLocation: string;
    dropoffLocation: string;
    type: string;
    mapURL:string;
  }
  
  // Update the Delivery interface to include the Job properties
  export interface Delivery {
    delivery_ID: string;
    delivery_Weight: number;
    jobs: Job; // Use the Job interface here
  }