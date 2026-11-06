# React + Vite

This template provides a minimal setup to get React working in Vite with HMR and some Oxlint rules.

Currently, two official plugins are available:

- [@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react) uses [Oxc](https://oxc.rs)
- [@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react-swc) uses [SWC](https://swc.rs/)

## Contact form setup

The contact form sends messages through EmailJS directly from the browser.

1. In EmailJS, connect the inbox that should receive messages and create an email template.
2. Set the template's **To email** to `{{to_email}}`, its subject to `{{subject}}`, and include `{{from_name}}`, `{{from_email}}`, and `{{message}}` in the email body. Set **Reply-To** to `{{reply_to}}`.
3. Copy `.env.example` to `.env.local` and fill in the service ID, template ID, and public key from your EmailJS account.
4. Restart the Vite development server after editing `.env.local`. Add the same variables to your hosting provider before deploying.
5. In EmailJS, restrict allowed origins to your website domain and configure its available anti-spam protections.

The configured template recipient is `vickyojha97@gmail.com`. The public key is intended for browser use; never put an EmailJS private key in frontend code.

## Publish for free with Netlify

The site is a static Vite app and can be published on Netlify's free plan. A free Netlify account is required; the default site address is a Netlify subdomain.

### Quick publish

1. Configure EmailJS as described above, then create `.env.local` from `.env.example` and fill in the three EmailJS values.
2. Run `npm run build` in the project folder.
3. Sign in to Netlify and open its manual deploy page at <https://app.netlify.com/drop>.
4. Upload the generated `dist` folder. Netlify will provide a public website URL.

The EmailJS public configuration is included in the built frontend. Do not put an EmailJS private key or other secret in `.env.local`.

### Automatic deploys from Git

Push this project to a Git provider, then import that repository into Netlify. The included `netlify.toml` configures the build command (`npm run build`) and publish directory (`dist`). Add `VITE_EMAILJS_SERVICE_ID`, `VITE_EMAILJS_TEMPLATE_ID`, and `VITE_EMAILJS_PUBLIC_KEY` under the site's environment variables in Netlify, then deploy. Set these variables in the build environment; adding them after a deploy requires a new deploy.

## React Compiler

The React Compiler is not enabled on this template because of its impact on dev & build performances. To add it, see [this documentation](https://react.dev/learn/react-compiler/installation).

## Expanding the Oxlint configuration

If you are developing a production application, we recommend using TypeScript with type-aware lint rules enabled. Check out the [TS template](https://github.com/vitejs/vite/tree/main/packages/create-vite/template-react-ts) for information on how to integrate TypeScript and Oxlint's TypeScript related rules in your project.
