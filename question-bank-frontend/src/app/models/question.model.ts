import { Option } from "./option.model";

export interface Question {
  id: number;
  statement: string;
  difficulty: EDifficultyLevel;
  topicId: number;
  options: Option[];
}

export interface AddQuestion {
  statement: string;
  difficulty: EDifficultyLevel;
  topicId: number;
}

export interface UpdateQuestion {
  id: number;
  statement: string;
  difficulty: EDifficultyLevel;
  topicId: number;
}

export interface SearchQuestion {
  statement?: string;
  difficulty?: EDifficultyLevel;
  topicId?: number;
}

export enum EDifficultyLevel {
  Easy = 'Easy',
  Medium = 'Medium',
  Hard = 'Hard'
}
