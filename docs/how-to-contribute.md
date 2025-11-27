# How To Contribute

## Local preview

Install dependencies and run the local dev server:

```bash
python -m pip install -r requirements.txt
mkdocs serve
```

Open `http://127.0.0.1:8000` in your browser to preview the docs.

## Adding or editing pages

- Edit or add markdown files under `docs/`.
- Update `mkdocs.yml` `nav:` if you add new top-level pages.
- Commit changes and push to the repository.

## Submitting game content

- Keep assets organized in a dedicated `assets/` folder in the repo.
- Include build/export scripts and a short README for how to run the build.
