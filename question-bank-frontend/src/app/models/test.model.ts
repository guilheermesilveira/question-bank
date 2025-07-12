import { Option } from "./option.model";
import { EDifficultyLevel, Question } from "./question.model";

export interface Test {
  id: number;
  title: string;
  totalQuestions: number;
  numberOfCorrectAnswers: number;
  status: ETestStatus;
  userId: number;
  testQuestions: TestQuestion[];
}

export interface CreateTest {
  userId: number;
  title: string;
  totalQuestions: number;
  difficulty: EDifficultyLevel;
  topicIds: number[];
}

export interface FinishTest {
  testId: number;
  answers: Answer[];
}

export interface TestQuestion {
  question: Question;
  selectedOption?: Option;
  isCorrect?: boolean;
}

export interface Answer {
  questionId: number;
  selectedOptionId: number;
}

export enum ETestStatus {
  InProgress = "InProgress",
  Finished = "Finished",
  Canceled = "Canceled"
}
