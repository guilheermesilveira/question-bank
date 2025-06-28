export interface Alternative {
  id: number;
  text: string;
  isCorrect: boolean;
  questionId: number;
}

export interface AddAlternative {
  text: string;
  isCorrect: boolean;
  questionId: number;
}

export interface UpdateAlternative {
  id: number;
  text: string;
  isCorrect: boolean;
  questionId: number;
}

export interface SearchAlternative {
  text?: string;
  isCorrect?: boolean;
  questionId?: number;
}
