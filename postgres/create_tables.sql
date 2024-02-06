CREATE TABLE Projects (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) UNIQUE,
    model VARCHAR(255)
);
CREATE TABLE Components (
    id SERIAL PRIMARY KEY,
    project_id INT,
    name VARCHAR(255),
    class VARCHAR(255),
    current FLOAT,
    voltage FLOAT,
    FOREIGN KEY (project_id) REFERENCES Projects(id)
);
CREATE TABLE Classes (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) UNIQUE
);
CREATE TABLE Tests (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255)
);