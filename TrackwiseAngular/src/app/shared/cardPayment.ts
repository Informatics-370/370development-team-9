import { OrderLines } from "./orderLines";

export interface NewCard {
    lastName: string;
    firstName: string;
    email: string;
    cardNumber: bigint;
    cardExpiry: number;
    cvv: number;
    vault: boolean;
    amount: number;
  }
  
  export interface CardPayment {
    completed: boolean;
    secure3DHtml: string | null;
    payRequestId: string;
    response: string;
  }

  export interface CheckoutRequest {
    orderVM: OrderLines;
    newCard: NewCard;
  }