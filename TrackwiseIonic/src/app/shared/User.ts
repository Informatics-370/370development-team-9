export interface User {
    token:{
        value:{
            token:string;
            user:string;
        }
    };
    role?: string
}