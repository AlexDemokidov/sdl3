CREATE ROLE "Alex" LOGIN PASSWORD 'alex1234';
GRANT USAGE ON SCHEMA public TO "Alex";
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO "Alex";
GRANT SELECT, INSERT, UPDATE, DELETE ON Projects TO "Alex";
GRANT SELECT, INSERT, UPDATE, DELETE ON Components TO "Alex";
GRANT SELECT, INSERT, UPDATE, DELETE ON Classes TO "Alex";
GRANT SELECT, INSERT, UPDATE, DELETE ON Tests TO "Alex";