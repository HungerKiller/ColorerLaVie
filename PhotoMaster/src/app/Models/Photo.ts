import { Label } from './Label';

export class Photo {
    Id: number;
    Date: Date;
    Path: string;
    Location: string;
    Description: string;
    Labels: Label[];
}