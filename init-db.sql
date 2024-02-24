CREATE TABLE IF NOT EXISTS "genre" (
    "id" SERIAL PRIMARY KEY,
    "name" VARCHAR(255) NOT NULL
);

INSERT INTO "genre" ("name") VALUES 
    ('Technology'),
    ('Science'),
    ('Health & Wellness'),
    ('Sports'),
    ('Music'),
    ('Literature'),
    ('Travel'),
    ('Food & Cooking'),
    ('Fashion'),
    ('Art & Design'),
    ('Gaming'),
    ('Education'),
    ('Anime'),
    ('Environment'),
    ('Business & Finance'),
    ('Movies & TV'),
    ('Comedy'),
    ('Lifestyle'),
    ('History'),
    ('DIY & Crafts');

CREATE TABLE IF NOT EXISTS "user" (
    "id" SERIAL PRIMARY KEY,
    "email" VARCHAR(255),
    "name" VARCHAR(255),
    "googleid" VARCHAR(255),
    "profilepicture" TEXT
);

CREATE TABLE IF NOT EXISTS "poll" (
    "id" SERIAL PRIMARY KEY,
    "userid" INTEGER NOT NULL REFERENCES "user" ("id"),
    "title" VARCHAR(255) NOT NULL,
    "firstoption" VARCHAR(255) NOT NULL,
    "secondoption" VARCHAR(255) NOT NULL,
    "genreid" INTEGER REFERENCES "genre" ("id")
);

CREATE TABLE IF NOT EXISTS "userpoll" (
    "id" SERIAL PRIMARY KEY,
    "userid" INTEGER NOT NULL REFERENCES "user" ("id"),
    "pollid" INTEGER NOT NULL REFERENCES "poll" ("id"),
    "vote" BOOLEAN
);
