export interface Option {
  id: number;
  text: string;
  isCorrect: boolean;
  questionId: number;
}

export interface AddOption {
  text: string;
  isCorrect: boolean;
  questionId: number;
}

export interface UpdateOption {
  id: number;
  text: string;
  isCorrect: boolean;
  questionId: number;
}

export interface SearchOption {
  text?: string;
  isCorrect?: boolean;
  questionId?: number;
}
