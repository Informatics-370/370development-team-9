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
  delivery_Status_ID: string;
  DeliveryStatus: DeliveryStatus;
  JobStatus:JobStatus;
  job_Status_ID:string;
  driver_ID:string;
  Driver:Driver;
  truckID:string;
  Truck:Truck;
  trailerID:string;
  Trailer:Trailer;
  job_ID:string;
  jobs: Job; // Use the Job interface here
}

export interface DeliveryStatus {
  delivery_Status_ID: string;
  name: string;
  description: string;
  Deliveries: Delivery[];
}
export interface JobStatus {
  job_Status_ID: string;
  name: string;
  description: string;
  Deliveries: Delivery[];
}

export interface Driver {
  driver_ID: string;
  name: string;
  Deliveries: Delivery[];
}
export interface Trailer {
  trailerID: string;
  name: string;
  Deliveries: Delivery[];
}
export interface Truck {
  truckID: string;
  name: string;
  Deliveries: Delivery[];
}
