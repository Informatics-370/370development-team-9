export interface MileageFuel {

    initial_Mileage: number;
    final_Mileage: number;
    total_Fuel: number;
    delivery_ID: string;
    mileage: number| null | undefined;
    fuel: number | null | undefined;
  }

export interface TruckData{
    registration: string;
    mFList: MileageFuel[];
}

export interface KiloLitres{
  truck_License:string;
  job_ID:string;
  kilosDriven: number;
  fuelInput: number;
}