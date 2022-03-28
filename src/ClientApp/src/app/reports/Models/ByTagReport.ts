export interface ByTagReport {
    byCount: LabelNumber[];
    byDollar: LabelNumber[];
    
}

export interface LabelNumber {
    name: string;
    value: number;
}