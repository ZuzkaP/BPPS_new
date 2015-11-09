ALTER TABLE feedback_questions ADD FOREIGN KEY (question_id) REFERENCES questions;
ALTER TABLE feedback_questions ADD FOREIGN KEY (feedback_id) REFERENCES feedbacks;
