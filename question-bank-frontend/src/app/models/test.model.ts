export interface Test {
  id: number;
  title: string;
  totalQuestions: number;
  numberOfCorrectAnswers: number;
  status: ETestStatus;
  userId: number;
}

export enum ETestStatus {
  InProgress = "InProgress",
  Finished = "Finished",
  Canceled = "Canceled"
}
