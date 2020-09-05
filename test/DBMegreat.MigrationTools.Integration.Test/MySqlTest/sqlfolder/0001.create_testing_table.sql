CREATE TABLE test_table (
  test_table_id int(11) NOT NULL,
  name varchar(100) NOT NULL,
  created_datetime datetime NOT NULL,
  PRIMARY KEY (test_table_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;